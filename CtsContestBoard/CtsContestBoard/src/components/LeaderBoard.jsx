import React from 'react';
import { Header, Image, Table, Card, Grid } from 'semantic-ui-react';


export default class LeaderBoard extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        const userlist = this.props.data.map((Board, i) => <User className="board-row" name={Board.Name} picture={Board.Picture} totalEarnedPoints={Board.TotalEarnedPoints} balance={Board.Balance} todayEarnedPoints={Board.TodayEarnedPoints} rank={i + 1} key={i} />);
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
    <Grid textAlign="center" className="colHeader">
        <Grid.Column width={1}>
            <h4>#</h4>
        </Grid.Column>
        <Grid.Column width={2}>
        </Grid.Column>
        <Grid.Column width={7}>
            <h4>Name</h4>
        </Grid.Column>
        <Grid.Column width={2}>
            <h4>Totally earned</h4>
        </Grid.Column>
        <Grid.Column width={2}>
            <h4>Balance</h4>
        </Grid.Column>
        <Grid.Column width={2} className="recent">
            <h4>Today's points</h4>
        </Grid.Column>
    </Grid>
    );

const User = ({picture, name, totalEarnedPoints, balance, todayEarnedPoints, rank}) => {
    return (
        <Grid className="users vcenter">
            <Grid.Column width={1} className="rank">
                <h4>{rank}</h4>
            </Grid.Column>
            <Grid.Column width={2} className="picture">
                <Image className="picture-leaderboard" src={picture} />
            </Grid.Column>
            <Grid.Column width={7} className="name">
                <h4>{name}</h4>
            </Grid.Column>
            <Grid.Column width={2} textAlign="center">
                <h4>{totalEarnedPoints}</h4>
            </Grid.Column>
            <Grid.Column width={2} textAlign="center">
                <h4>{balance}</h4>
            </Grid.Column>
            <Grid.Column width={2} textAlign="center">
                <h4>{todayEarnedPoints}</h4>
            </Grid.Column>
        </Grid>
    )
}

