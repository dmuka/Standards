import { useCookies } from "react-cookie";

const ACCESS_TOKEN_COOKIE_NAME = "accessToken";
const REFRESH_TOKEN_COOKIE_NAME = "refreshToken";

export function useTokens() {
  const [cookies, setCookie, removeCookie] = useCookies([
    ACCESS_TOKEN_COOKIE_NAME,
    REFRESH_TOKEN_COOKIE_NAME,
  ]);

  const getAccessToken = () => cookies[ACCESS_TOKEN_COOKIE_NAME];
  const setAccessToken = (token) =>
    setCookie(ACCESS_TOKEN_COOKIE_NAME, token, { path: "/" });
  const getRefreshToken = () => cookies[REFRESH_TOKEN_COOKIE_NAME];
  const setRefreshToken = (token) =>
    setCookie(REFRESH_TOKEN_COOKIE_NAME, token, { path: "/" });
  const removeTokens = () => {
    removeCookie(ACCESS_TOKEN_COOKIE_NAME);
    removeCookie(REFRESH_TOKEN_COOKIE_NAME);
  };

  const checkTokenExpiration = () => {
    const accessToken = getAccessToken();

    if (!accessToken) {
      // No access token found, user is not authenticated
      return;
    }

    const tokenPayload = decodeToken(accessToken);
    const expirationTime = tokenPayload.exp * 1000; // Convert expiration time to milliseconds

    if (expirationTime < Date.now()) {
      // Access token has expired, perform token refresh
      const refreshToken = getRefreshToken();

      if (!refreshToken) {
        // No refresh token found, user needs to reauthenticate
        removeTokens();
        return;
      }

      // Perform token refresh request to the backend
      fetch("https://api.example.com/refresh-token", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ refresh_token: refreshToken }),
      })
        .then((response) => response.json())
        .then((data) => {
          if (data.access_token) {
            // Token refresh successful, update access token
            setAccessToken(data.access_token);
          } else {
            // Token refresh failed, user needs to reauthenticate
            removeTokens();
          }
        })
        .catch((error) => {
          // Handle the error
        });
    }
  };

  const decodeToken = (token) => {
    try {
      const base64Url = token.split(".")[1];
      const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split("")
          .map((c) => {
            return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
          })
          .join("")
      );

      return JSON.parse(jsonPayload);
    } catch (error) {
      // Error decoding token
      return null;
    }
  };

  return {
    getAccessToken,
    setAccessToken,
    getRefreshToken,
    setRefreshToken,
    removeTokens,
    checkTokenExpiration,
  };
}
