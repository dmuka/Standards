import React from "react";
import ApiAuthorizationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import AuthProvider from "./components/api-authorization/AuthProvider";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { CookiesProvider } from "react-cookie";
import AppRoutes from "./AppRoutes";
import AuthorizeRoute from "./components/api-authorization/AuthorizeRoute";
import Layout from "./components/Layout";
import "./custom.css";

const router = createBrowserRouter([
  {
    // parent
    element: <Layout />,
    children: [...AppRoutes, ...ApiAuthorizationRoutes],
  },
]);

export default function App() {
  return (
    <CookiesProvider>
      <AuthProvider>
        <RouterProvider
          router={router}
          fallbackElement={<p>Initial Load...</p>}
        />
      </AuthProvider>
    </CookiesProvider>
  );
}
