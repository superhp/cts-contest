import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Tasks } from './components/Tasks';
import { About } from './views/About';
import { Prizes } from './components/Prizes';
import { TaskComponent } from './components/Task';

export const routes = <Layout>
    <Route exact path='/' component={ Tasks } />
    <Route path='/about' component={About} />
    <Route path='/tasks' component={Tasks} />
    <Route path='/prizes' component={Prizes} />
    <Route path="/task/:id" component={(props: any)=> <TaskComponent {...props} /> } />
</Layout>;
