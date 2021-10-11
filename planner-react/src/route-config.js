import IndexProjects from "./Components/IndexProjects";

import LandingPage from "./Utils/LandingPage";
import RedirectToLandingPage from './Utils/RedirectToLandingPage';

const routes = [
    {path: '/Components', component: IndexProjects, exact: true},
   

    {path: '/', component: LandingPage, exact: true},
    {path: '*', component: RedirectToLandingPage}
];