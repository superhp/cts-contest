import React from 'react';
import dotnetify from 'dotnetify';
import Slogan from './views/Slogan.jsx';
import Information from './views/Information.jsx';
import LeaderBoard from './views/LeaderBoard.jsx';
import PrizeBoard from './views/PrizeBoard.jsx';
import SpecialPrizeBoard from './views/SpecialPrizeBoard.jsx';
import Header from './components/Header.jsx';
import JobPosters from './views/JobPosters.jsx';

const timer = {
    dayGame: {
        day: (new Date().getDate()),
        hour: 15, 
        minute: 30
    },
    conferenceGame: {
        day: new Date().getDate(),
        /*hour: 16,
        minute: 45*/
        //day: (new Date().getDate()),
        hour: 15,
        minute: 30
    }
}
export const BoardEnum = {
    LeaderBoard: 0,
    Prizes: 1,
    TodayPrizes: 2,
    WeekPrizes: 3,
    Information: 4,
    JobPosters: 5,
    Slogan: 6,
    Duels: 7
}
class Board extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("BoardLoader", this);
        this.state = { Board: 1, PrizesForPoints: [], LeaderBoard: [], WeeksPrize: {}, TodaysPrize: {} }
    }
    renderSlide() {
        //switch (BoardEnum.Information) {
        console.log(this.state.Board);
        //return <PrizeBoard prizes={this.state.PrizesForPoints}/>;
        switch (this.state.Board) {
            
            case BoardEnum.LeaderBoard:
                const sortedLeaderboard = this.state.LeaderBoard;
                sortedLeaderboard.sort((a,b) => {
                    /*if(a.TodaysBalance < b.TodaysBalance)
                        return 1;
                    else if(a.TodaysBalance > b.TodaysBalance)
                        return -1
                    else return 0;*/
                    if (a.TotalBalance < b.TotalBalance)
                        return 1;
                    else if (a.TotalBalance > b.TotalBalance)
                        return -1;
                    else return 0;
                });
                return <LeaderBoard data={sortedLeaderboard}/>;
            case BoardEnum.Prizes:
                return <PrizeBoard prizes={this.state.PrizesForPoints}/>;
            case BoardEnum.TodayPrizes:
                var leaders = [];
                if (this.state.TodaysPrize.Applicants !== null && this.state.TodaysPrize.Applicants !== undefined)
                    leaders = this.state.TodaysPrize.Applicants.map(user => ({ username: user.Name, points: user.TodaysBalance, picture: user.Picture }));
                for(let i = 0; i < 3; i++)
                    leaders.push({ username: undefined, points: '', picture: undefined });
                return <SpecialPrizeBoard data={leaders} prize={this.state.TodaysPrize} board="Todays' prize"/>;
         /*   case BoardEnum.WeekPrizes:
                var leaders = [];
                if(this.state.WeeksPrize.Applicants !== null && this.state.WeeksPrize.Applicants !== undefined)
                    leaders = this.state.WeeksPrize.Applicants.map(user => ({ username: user.Name, points: user.TotalBalance, picture: user.Picture }));
                for(let i = 0; i < 3; i++)
                     leaders.push({ username: undefined, points: '', picture: undefined });

                return <SpecialPrizeBoard data={leaders} prize={this.state.WeeksPrize} board='Conference prize'/>;*/
            case BoardEnum.Information:
                return <Information timer={timer} />;
            case BoardEnum.JobPosters:
                return <JobPosters/>;
            /* case BoardEnum.Slogan:
                 return <Slogan />;
             case BoardEnum.Duels:
                 var leaders = [];
                 if (this.state.DuelsStatistics.Applicants !== null && this.state.DuelsStatistics.Applicants !== undefined)
                     leaders = this.state.DuelsStatistics.Applicants.map(user => ({ username: user.Name, points: user.TodaysBalance, picture: user.Picture }));
                 for(let i = 0; i < 3; i++)
                     leaders.push({ username: undefined, points: '', picture: undefined });
                 return <SpecialPrizeBoard data={leaders} prize={this.state.DuelsStatistics} board='Duels won'/>;*/
        }

    }
    render() {
        return (
            <div>
                <Header board={this.state.Board} timer={timer}/>
                <div>
                    {this.renderSlide()}
                </div>
            </div>
        );
    }
}

export default Board; 