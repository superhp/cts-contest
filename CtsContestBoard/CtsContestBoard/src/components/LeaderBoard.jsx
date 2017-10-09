import React from 'react';
import { Header, Image, Table, Card, Grid } from 'semantic-ui-react';
//import '../css/leaderboardStyle.css';


export default class LeaderBoard extends React.Component {
    constructor(props) {
        super(props);
        console.log(props);
    }
    render() {
        const userlist = this.props.data.map((Board, i) => <User username={Board.Name} score={Board.Score} rank={i+1} key={i} />);
        return (
            <div className="container">
                <LeaderboardHeader />
                <ColumnHeader />
                <div>
                    {userlist}
                </div>
            </div>
        )
    }

}

const LeaderboardHeader = () => {
    return (
        <div className="leadheader">
            <h2>Leaderboard</h2>
        </div>
    )
}

const ColumnHeader = () => (
    <Grid textAlign="center">
        <Grid.Column width={2}>
            <h4>#</h4>
        </Grid.Column>
        <Grid.Column width={10}>
            <h4>Name</h4>
        </Grid.Column>
        <Grid.Column width={4} className="recent">
            <h4>Score</h4>
        </Grid.Column>
    </Grid>
    );

const User = ({ username, score, rank}) => {
    return (
        <Grid className="users vcenter">
            <Grid.Column width={2} className="rank">
                <h4>{rank}</h4>
            </Grid.Column>
            <Grid.Column width={10} className="name">
                <h4>{username}</h4>
            </Grid.Column>
            <Grid.Column width={4} textAlign="center">
                <h4>{score}</h4>
            </Grid.Column>
        </Grid>
    )
}

