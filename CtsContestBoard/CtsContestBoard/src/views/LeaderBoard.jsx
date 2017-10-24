import React from 'react';
import { Header, Image, Table, Card, Grid } from 'semantic-ui-react';


export default class LeaderBoard extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        const userlist = this.props.data.map((Board, i) => <User className="board-row" name={Board.Name} picture={Board.Picture} todaysBalance={Board.TodaysBalance} totalBalance={Board.TotalBalance} rank={i + 1} key={i} />);
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
        <Grid.Column width={3}>
            <h4>Total balance</h4>
        </Grid.Column>
        <Grid.Column width={3} className="recent">
            <h4>Today's points</h4>
        </Grid.Column>
    </Grid>
    );

const User = ({ picture, name, totalBalance, todaysBalance, rank}) => {
    return (
        <Grid className="users vcenter">
            <Grid.Column width={1} className="rank">
                <h4>{rank}</h4>
            </Grid.Column>
            <Grid.Column width={2} className="picture">
                <Image className="picture-leaderboard" src={picture ? picture : 'img/186382-128.png'} />
            </Grid.Column>
            <Grid.Column width={7} className="name">
                <h4>{name}</h4>
            </Grid.Column>
            <Grid.Column width={3} textAlign="center">
                <h4>{totalBalance}</h4>
            </Grid.Column>
            <Grid.Column width={3} textAlign="center">
                <h4>{todaysBalance}</h4>
            </Grid.Column>
        </Grid>
    )
}

