import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './views/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { About } from './views/About';

export const routes = <Layout>
    <Route exact path='/' component={ Home } />
    <Route path='/counter' component={ Counter } />
    <Route path='/fetchdata' component={FetchData} />
    <Route path='/about' component={About} />
</Layout>;
