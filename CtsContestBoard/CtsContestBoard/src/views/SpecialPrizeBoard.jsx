import React from 'react';
import { Grid } from 'semantic-ui-react';
import ReactCountdownClock from 'react-countdown-clock';

import UserCard from '../components/UserCard.jsx';
import WeekPrizeCard from '../components/WeekPrizeCard.jsx';

export default class SpecialPrizeBoard extends React.Component {
    constructor(props) {
        super(props);

    }

    render() {
        return (
            <div>
                <h1 className='prize-board-header'>
                    {
                        this.props.board === 'today'
                        ? 'Today\'s prize'
                        : 'Conference prize'
                    }    
                </h1>
                <OnePrizeBoard data={this.props.data} prize={this.props.prize} state={this.state} />
            </div>
        );
    }

}

const TwoPrizeBoard = ({ data }) => (
    <Grid id="special-prize-board">
        <Grid.Row columns={3} className="first-week-prize-row">
            <Grid.Column width={5} className="full-space">
                <Grid.Row className="first-prize-card-row">
                    <WeekPrizeCard className="weekPrizeCard" name="IPhone X" picture="https://www.telia.lt/documents/20184/5622585/Nokia_3310_BU_2_624x750/595c7466-8cf0-3699-d72f-1554ce26cce2?t=1499753422524" />
                </Grid.Row>
                <Grid.Row className="second-prize-card-row">
                    <WeekPrizeCard className="weekPrizeCard" name="IPhone X" picture="https://www.telia.lt/documents/20184/5622585/Nokia_3310_BU_2_624x750/595c7466-8cf0-3699-d72f-1554ce26cce2?t=1499753422524" />
                </Grid.Row>
            </Grid.Column>
            <Grid.Column width={7}>

            </Grid.Column>
            <Grid.Column width={4}>

                {/* <ReactCountdownClock seconds={86400}
                    color="#000"
                    alpha={0.9}
                    size={300}
                    onComplete={() => { }} /> */}
            </Grid.Column>
        </Grid.Row>
        <Grid.Row columns={2} className="second-week-prize-row">
            <Grid.Column width={5} className="white-space">
            </Grid.Column>
            <Grid.Column width={11}>
                <Podium first={data[0]} second={data[1]} third={data[2]} />
            </Grid.Column>
        </Grid.Row>
    </Grid>
)

const OnePrizeBoard = ({ data, prize, state }) => (
    <Grid id="special-prize-board">
        <Grid.Row columns={2} className="second-week-prize-row">
            <Grid.Column width={5} >
                <div style={{ width: '75%' }}>
                    {
                        prize.Name 
                        ? <WeekPrizeCard className="singleWeekPrizeCard" name={prize.Name} picture={prize.Picture} />
                        : ''
                    } 
                </div>
            </Grid.Column>
            <Grid.Column width={11}>
                <Podium first={data[0]} second={data[1]} third={data[2]} />
            </Grid.Column>
        </Grid.Row>
    </Grid>
)

const Podium = ({ first, second, third }) => (
    <Grid id="podiumDiv">
        <Grid.Column width={1} />
        <Grid.Column width={14}>
            <Grid className="podium">
                <Grid.Column width={5} className="secondPlace podium-step">
                    <UserCard place="second" username={second.username} points={second.points} picture={second.picture}/>
                </Grid.Column>
                <Grid.Column width={6} className="firstPlace podium-step">
                    <UserCard place="first" username={first.username} points={first.points} picture={first.picture} />
                </Grid.Column>
                <Grid.Column width={5} className="thirdPlace podium-step">
                    <UserCard place="third" username={third.username} points={third.points} picture={third.picture} />
                </Grid.Column>
            </Grid>
        </Grid.Column>
        <Grid.Column width={1} />
    </Grid>
)