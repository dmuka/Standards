import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import Routes from './Routes';
import Housings from "./components/Housings/Housings";
import { Rooms } from "./components/Housings/Rooms";
import { Home } from "./components/Home";

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
    ...ApiAuthorizationRoutes
];

export default AppRoutes;