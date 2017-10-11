import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { FetchData } from './components/FetchData';
import { Tasks } from './views/Tasks';
import { About } from './views/About';
import { Prizes } from './views/Prizes';
import { TaskComponent } from './views/Task';

export class Routes extends React.Component<any, any> {
    constructor(props:any) {
        super(props);

        this.state = {
            userInfo: {
                isLoggedIn: false,
                name: '',
                balance: 0
            }
        }

        this.incrementBalance = this.incrementBalance.bind(this);
        this.decrementBalance = this.decrementBalance.bind(this);
    }
    componentDidMount() {
        fetch('api/User', {
            credentials: 'include'
        })
            .then(response => response.json() as Promise<UserInfo>)
            .then(data => {
                this.setState({userInfo: data});
            });
    }

    incrementBalance(value:number){
        const userInfo = this.state.userInfo;
        userInfo.balance = userInfo.balance + value;
        this.setState({userInfo});
    }

    decrementBalance(value:number){
        const userInfo = this.state.userInfo;
        userInfo.balance = userInfo.balance - value;
        this.setState({userInfo});
    }

    render() {
        return (
            <Layout userInfo={this.state.userInfo}>
                <Route exact path='/' component={Tasks} />
                <Route path='/about' component={About} />
                <Route path='/prizes' render={(props:any) => <Prizes {...props} userInfo={this.state.userInfo} onDecrementBalance={this.decrementBalance}/>} />
                <Route path="/task/:id" render={(props: any) => <TaskComponent {...props} userInfo={this.state.userInfo} onIncrementBalance={this.incrementBalance}/>} />
            </Layout>
        )
    }
}
