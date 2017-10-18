import React from 'react';
import dotnetify from 'dotnetify';
import LeaderBoard from './components/LeaderBoard.jsx';
import PrizeBoard from './components/PrizeBoard.jsx';
import SpecialPrizeBoard from './components/SpecialPrizeBoard.jsx';
import Header from './components/Header.jsx';

class Board extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("BoardLoader", this);
        this.state = { Board: 1, Prizes: [], LeaderBoard: [], WeekPrizes: [] }
    }

    render() {
        const modifiedWeekPrizes = this.state.WeekPrizes.map(x => ({ username: x.Applicant.Name, points: x.Applicant.Balance, picture: x.Applicant.Picture }));
        console.log(modifiedWeekPrizes);
        modifiedWeekPrizes.push({ username: undefined, points: -1, picture: undefined });
        modifiedWeekPrizes.push({ username: undefined, points: -1, picture: undefined });
        modifiedWeekPrizes.push({ username: undefined, points: -1, picture: undefined });
        return (
            <div>
                <Header />
                <div>
                    {
                        true ?
                            <LeaderBoard data={this.state.LeaderBoard} /> :
                            <SpecialPrizeBoard data={modifiedWeekPrizes} />
                            
                    }
                </div>
            </div>
        );
    }
}

export default Board; 