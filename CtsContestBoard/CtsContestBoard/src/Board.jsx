import React from 'react';
import dotnetify from 'dotnetify';

class Board extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("BoardLoader", this);

        this.state = { Board: "", A: "", LastUpdate: "", ShowLeaderboard: false }
    }

    render() {
        return (
            <div>
                <div>
                    A: {this.state.A}
                </div>
                <div>
                    LastUpdate: {this.state.LastUpdate}
                </div>
                <div>
                    Show leaderboard: {this.state.ShowLeaderboard ? "true" : "false"}
                </div>
            </div>
        );
    }
}

export default Board; 