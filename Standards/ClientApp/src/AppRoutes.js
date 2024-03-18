import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import Housings from "./components/Housings/Housings";
import { Rooms } from "./components/Housings/Rooms";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/housings/list',
    element: <Housings />
  },
  {
    path: '/rooms/list',
    element: <Rooms />
  },
  ...ApiAuthorizationRoutes
];

export default AppRoutes;
