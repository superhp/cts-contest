import 'semantic-ui-css/semantic.min.css';
import './css/site.css';
import './css/brand.css';
import 'bootstrap';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { BrowserRouter, Router, Route } from 'react-router-dom';
import createHistory from 'history/createBrowserHistory';
import {Routes} from './routes';

import * as GA from 'react-ga';
GA.initialize('UA-109707377-1');

function logPageView(){
    GA.pageview(window.location.pathname + window.location.search);
}

const history = createHistory()
history.listen((location, action) => {
    logPageView();
});

function renderApp() {
    // This code starts up the React app when it runs in a browser. It sets up the routing
    // configuration and injects the app into a DOM element.
    const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')!;
    logPageView();
    ReactDOM.render(
        <AppContainer>
            <Router history={history} >
                <Routes />
            </Router>
        </AppContainer>,
        document.getElementById('react-app')
    );
}

renderApp();

// Allow Hot Module Replacement
if (module.hot) {
    module.hot.accept('./routes', () => {
        //routes = require<typeof RoutesModule>('./routes').routes;
        renderApp();
    });
}
