import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import Routes from './Routes';
import Housings from "./components/Housings/Housings";
import { Rooms } from "./components/Housings/Rooms";
import { Home } from "./components/Home";
import SignUp from "./components/api-authorization/SignUp";

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
    {
        path: ApplicationPaths.Register,
        element: <SignUp />
    },
    ...ApiAuthorizationRoutes
];

export default AppRoutes;