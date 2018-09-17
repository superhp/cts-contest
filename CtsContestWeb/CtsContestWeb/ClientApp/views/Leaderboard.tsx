import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import 'isomorphic-fetch';
import { Grid, Icon, Table, Container, Header, Divider, Loader } from 'semantic-ui-react';
import * as _ from 'lodash';
import { UserInfo } from '../components/models/UserInfo';
import { Prize } from '../components/PurchaseModal';
//import * as GA from 'react-ga';
//GA.initialize('UA-109707377-1');

interface LeaderboardState {
	users: UserInfo[];
	prizes: Prize[];
    loading: boolean;
}

export class Leaderboard extends React.Component<any, LeaderboardState> {
    _mounted: boolean;
    constructor() {
		super();
		this.state = { users: [], loading: true, prizes: [] };

        fetch('api/User/Users')
            .then(response => response.json() as Promise<UserInfo[]>)
            .then(data => {
                if (this._mounted)
                    this.setState({ users: data, loading: false });

			});

		fetch('api/Prize/GetWinnables')
			.then(response => response.json() as Promise<Prize[]>)
			.then(data => {
				if (this._mounted) {
					let prizes: Prize[] = []; 
					data.forEach(prize => {
						for (let i = 0; i < prize.quantity; i++) {
							prizes.push(prize); 
						}
					})
					_.sortBy(prizes, 'price', 'asc').reverse();
					this.setState({ prizes: prizes, loading: false });					
				}
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
			: Leaderboard.renderLeaderboard(this.state.users, this.state.prizes);

        return (
            <div>
                <div className='cg-page-header'>
                    <div className='cg-page-header-overlay'>
                        <Container fluid>
                            <Header as='h1' textAlign='center' inverted>
                                <Icon name='numbered list' />
                                <Header.Content>
                                    Leaderboard
                            </Header.Content>
                            </Header>
                        </Container>
                    </div>
                </div>
                <Container>
                    {contents}
                </Container>

                <div style={{ height: 10 }}></div>
            </div>
        )
    }

    private static renderLeaderboard(users: UserInfo[], prizes: Array<Prize>) {
        users = _.sortBy(users, 'totalBalance', 'asc').reverse();

		const userlist = users.map((user, i) => Leaderboard.renderUserRow(user, i + 1, i < prizes.length ? prizes[i] : undefined));
        return (
            <div className="container">
                <ColumnHeader />
                <div className='user-list'>
                    {userlist}
                </div>
            </div>
            );
    }

    private static renderUserRow(user: UserInfo, rank: number, prize: Prize | undefined) {
        return (
			<Grid key={rank} className="users vcenter">
				<Grid.Column width={2} className="rank">
					{prize ? <a href="/prizes"><img src={prize.picture} alt={prize.name} /></a> : ""}
				</Grid.Column>
                <Grid.Column width={2} className="rank">
                    <span>{rank}</span>
                </Grid.Column>
                <Grid.Column width={7} className="name">
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
			<h4></h4>
		</Grid.Column>
        <Grid.Column width={2}>
            <h4>#</h4>
        </Grid.Column>
        <Grid.Column width={7}>
            <h4>Name</h4>
        </Grid.Column>
        <Grid.Column width={4}>
            <h4>Total balance</h4>
        </Grid.Column>
    </Grid>
);