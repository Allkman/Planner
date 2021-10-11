import React, { Component } from "react";
import './App.css';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link,
  useRouteMatch,
  useParams,
  NavLink
} from "react-router-dom";
import LoginPage from "./Page/LoginPage";
import ProjectPage from "./Page/ProjectPage";
import StagePage from "./Page/StagePage";



class App extends Component {
  render() {
    return (
      <Router>
        {/* A <Switch> looks through its children <Route>s and
            renders the first one that matches the current URL. */}
        <div className="App container">
          <h3 className="d-flex justify-content-center m-3"></h3>

          <nav className="navbar navbar-expand-sm lb-light navbar-dark">
            <ul className="navbar-nav">
              <li className="nav-item- m-1">
                <NavLink
                  className="btn btn-light btn-outline-primary"
                  to="/login"
                >
                  Login Page
                </NavLink>
              </li>
              <li className="nav-item- m-1">
                <NavLink
                  className="btn btn-light btn-outline-primary"
                  to="/project"
                >
                  Project Page
                </NavLink>
              </li>
              <li className="nav-item- m-1">
                <NavLink
                  className="btn btn-light btn-outline-primary"
                  to="/stage"
                >
                  Stage Page
                </NavLink>
              </li>
            </ul>
          </nav>
        </div>
        <Switch>
          <Route path="/login" component={LoginPage} />
          <Route path="/project" component={ProjectPage} />
          <Route path="/stage" component={StagePage} />
        </Switch>
      </Router>
    );
  }
}

export default App;