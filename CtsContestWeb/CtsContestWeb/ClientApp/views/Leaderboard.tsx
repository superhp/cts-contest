import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import 'isomorphic-fetch';
import { Grid, Icon, Table, Container, Header, Divider, Loader } from 'semantic-ui-react';
import * as _ from 'lodash';
//import { UserInfo } from '../components/models/UserInfo';
//import * as GA from 'react-ga';
//GA.initialize('UA-109707377-1');

interface LeaderboardState {
    users: UserInfo[];
    loading: boolean;
}

export class Leaderboard extends React.Component<any, LeaderboardState> {
    _mounted: boolean;
    constructor() {
        super();
        this.state = { users: [], loading: true };

        fetch('api/User/Users')
            .then(response => response.json() as Promise<UserInfo[]>)
            .then(data => {
                if (this._mounted)
                    this.setState({ users: data, loading: false });

            });
    }
    componentWillMount() {
        //GA.pageview(window.location.pathname + window.location.search);
    }
    componentDidMount() {
        this._mounted = true;
    }
    componentWillUnmount() {
        this._mounted = false;
    }
    public render() {
        let contents = this.state.loading
            ? <Loader active inline='centered'>Loading</Loader>
            : Leaderboard.renderLeaderboard(this.state.users);

        return (
            <div>
                <div className='cg-page-header'>
                    <Container fluid>
                        <Header as='h1' textAlign='center' inverted>
                            <Icon name='numbered list' />
                            <Header.Content>
                                Leaderboard
                        </Header.Content>
                        </Header>
                    </Container>
                </div>
                <Container>
                    {contents}
                </Container>

                <div style={{ height: 10 }}></div>
            </div>
        )
    }

    private static renderLeaderboard(users: UserInfo[]) {
        users = _.sortBy(users, 'totalBalance', 'asc').reverse();

        const userlist = users.map((user, i) => Leaderboard.renderUserRow(user, i+1));
        return (
            <div className="container">
                <ColumnHeader />
                <div className='user-list'>
                    {userlist}
                </div>
            </div>
            );
    }

    private static renderUserRow(user: UserInfo, rank: number) {
        return (
            <Grid className="users vcenter">
                <Grid.Column width={2} className="rank">
                    <span>{rank}</span>
                </Grid.Column>
                <Grid.Column width={9} className="name">
                    <span>{user.name}</span>
                </Grid.Column>
                <Grid.Column width={4} textAlign="center">
                    <span>{user.totalBalance}</span>
                </Grid.Column>
            </Grid>
        )
    }
    
}

const ColumnHeader = () => (
    <Grid className="colHeader">
        <Grid.Column width={2}>
            <h4>#</h4>
        </Grid.Column>
        <Grid.Column width={9}>
            <h4>Name</h4>
        </Grid.Column>
        <Grid.Column width={4}>
            <h4>Total balance</h4>
        </Grid.Column>
    </Grid>
);