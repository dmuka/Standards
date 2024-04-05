import React from 'react'
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes'
// import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants'
import Routes from './Routes'
import Housings from './components/Housings/Housings'
import { Rooms } from './components/Housings/Rooms'
import { Home } from './components/Home'
import Sign from './components/api-authorization/Sign'
import { Login } from './components/api-authorization/Login'

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: Routes.HOUSINGS_LIST,
    element: <Housings />
  },
  {
    path: Routes.ROOMS_LIST,
    element: <Rooms />
  },
  //{
  //  path: ApplicationPaths.Register,
  //  element: <Login action="register" />
  //},
  //{
  //  path: ApplicationPaths.Login,
  //  element: <Login action="login" />
  //},
  ...ApiAuthorizationRoutes
]

export default AppRoutes
