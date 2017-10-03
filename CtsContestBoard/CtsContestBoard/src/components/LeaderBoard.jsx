import React from 'react';
import { Header, Image, Table, Card } from 'semantic-ui-react';
//import '../css/leaderboardStyle.css';


export default class LeaderBoard extends React.Component {
    constructor(props) {
        super(props);
        console.log(props);
    }
    render() {
        let userlist = this.props.data.map((Board, i) => <User username={Board.Name} score={Board.Score} rank={i}/>);
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
    <div className="row colheader">
        <div className="col-xs-2">
            <h4>#</h4>
        </div>
        <div className="col-xs-7">
            <h4>Name</h4>
        </div>
        <div className="col-xs-3 recent">
            <h4>Score</h4>
        </div>
    </div>
    );

const User = ({ username, score, rank}) => {
    return (
        <div className="row users  vcenter">
            <div className="col-xs-2 rank">
                <h4>{rank}</h4>
            </div>
            <div className="col-xs-7 name">
                <h4>{username}</h4>
            </div>
            <div className="col-xs-3">
                <h4>{score}</h4>
            </div>
        </div>
    )
}

