import { useContext, createContext } from "react";
import { useNavigate } from "react-router-dom";
import { useCookies } from 'react-cookie';

const AuthContext = createContext();

function AuthProvider ({ children }) {
  const [cookies, setCookie] = useCookies(['userId','accessToken','refreshToken']);
  //const navigate = useNavigate();

  async function loginAction (data) {
    try {
      const response = await fetch("api/auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      const request = await response.json();

      if (request.id) {
        setCookie('userId', request.id);
        setCookie('accessToken', request.accessToken);
        setCookie('refreshToken', request.refreshToken);
        //navigate("/home");
        return;
      }
      throw new Error(request.message);
    } catch (err) {
      console.error(err);
    }
  };

  function logOut () {
    setUser(null);
    setAccessToken('');
    setRefreshToken('');

    //navigate("/login");
  };

  return (
    <AuthContext.Provider value={{ cookies, loginAction, logOut }}>
      {children}
    </AuthContext.Provider>
  );
};

export function useAuth () {
  return useContext(AuthContext);
};

export default AuthProvider;