import React from 'react';
import dotnetify from 'dotnetify';
import LeaderBoard from './components/LeaderBoard.jsx';
import PrizeBoard from './components/PrizeBoard.jsx';
import SpecialPrizeBoard from './components/SpecialPrizeBoard.jsx';

class Board extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("BoardLoader", this);
        this.state = { Board: [], Prizes: [], WeekPrizes: [], ShowLeaderboard: false }
    }

    render() {
        return (
            <div>
                <div>
                    {
                        this.state.ShowLeaderboard&&false ?
                            <LeaderBoard data={this.state.Board} /> :
                            <SpecialPrizeBoard data={this.state.WeekPrizes} /> 
                    }
                </div>
            </div>
        );
    }
}

export default Board; 