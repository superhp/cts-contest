import React from 'react';
import dotnetify from 'dotnetify';
import LeaderBoard from './views/LeaderBoard.jsx';
import PrizeBoard from './views/PrizeBoard.jsx';
import SpecialPrizeBoard from './views/SpecialPrizeBoard.jsx';
import Header from './components/Header.jsx';

const BoardEnum = {
    LeaderBoard: 0,
    Prizes: 1,
    TodayPrizes: 2,
    WeekPrizes: 3
}
class Board extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("BoardLoader", this);
        this.state = { Board: 1, PrizesForPoints: [], LeaderBoard: [], WeeksPrize: { Name: 'untitled', Pircure: ''} }
    }

    renderSlide() {
        console.log(this.state);
        //switch (BoardEnum.WeekPrizes) {
        switch(this.state.Board){
            case BoardEnum.LeaderBoard:
                const sortedLeaderboard = this.state.LeaderBoard;
                sortedLeaderboard.sort((a,b) => {
                    if(a.TodaysBalance < b.TodaysBalance)
                        return 1;
                    else if(a.TodaysBalance > b.TodaysBalance)
                        return -1
                    else return 0;
                });
                return <LeaderBoard data={this.state.LeaderBoard} />
            case BoardEnum.Prizes:
                return <PrizeBoard prizes={this.state.PrizesForPoints} />
            case BoardEnum.TodayPrizes:
                var leaders = [];
                if (this.state.TodaysPrize.Applicants !== null && this.state.TodaysPrize.Applicants !== undefined)
                    leaders = this.state.TodaysPrize.Applicants.map(user => ({ username: user.Name, points: user.TodaysBalance, picture: user.Picture }));
                
                leaders.push({ username: undefined, points: -1, picture: undefined });
                leaders.push({ username: undefined, points: -1, picture: undefined });
                leaders.push({ username: undefined, points: -1, picture: undefined });
                return <SpecialPrizeBoard data={leaders} prize={this.state.TodaysPrize} board='today'/>
            case BoardEnum.WeekPrizes:
                leaders = [];
                if(this.state.WeeksPrize.Applicants !== null && this.state.WeeksPrize.Applicants !== undefined)
                    leaders = this.state.WeeksPrize.Applicants.map(user => ({ username: user.Name, points: user.TotalBalance, picture: user.Picture }));
                leaders.push({ username: 'Žmogus žmogenis', points: -1, picture: 'img/186382-128.png' });
                leaders.push({ username: 'Labas rytas', points: -1, picture: 'img/186382-128.png' });
                leaders.push({ username: undefined, points: -1, picture: undefined });
                return <SpecialPrizeBoard data={leaders} prize={this.state.WeeksPrize} board='week'/>
        }

    }
    render() {
        return (
            <div>
                <Header />
                <div>
                    {this.renderSlide()}
                </div>
            </div>
        );
    }
}

export default Board; 