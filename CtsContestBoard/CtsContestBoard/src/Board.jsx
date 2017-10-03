import React from 'react';
import dotnetify from 'dotnetify';
import LeaderBoard from './components/LeaderBoard.jsx';
import PrizeBoard from './components/PrizeBoard.jsx';

class Board extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("BoardLoader", this);
        this.state = { Board: "", Prizes: "", ShowLeaderboard: false }
    }

    render() {
        return (
            <div>
                <div>
                    {
                        this.state.ShowLeaderboard ?
                            <LeaderBoard data={this.state.Board} /> :
                            <PrizeBoard /> 
                    }
                </div>
            </div>
        );
    }
}

export default Board; 