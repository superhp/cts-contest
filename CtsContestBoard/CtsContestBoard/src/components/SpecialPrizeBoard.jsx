import React from 'react';
import UserCard from './UserCard.jsx';
import { Grid } from 'semantic-ui-react';

export default class SpecialPrizeBoard extends React.Component {
    render() {
        return (
            <Grid id="special-prize-board">
                <Grid.Row columns={1}>
                    <Grid.Column>
                        
                    </Grid.Column>
                </Grid.Row>
                <Grid.Row columns={1}>
                    <Grid.Column>
                        <Podium first={this.props.data[0]} second={this.props.data[1]} third={this.props.data[2]} />
                    </Grid.Column>
                </Grid.Row>
            </Grid>
        );
    }

}

const Podium = ({ first, second, third}) => (
    <Grid id="podiumDiv">
        <Grid.Column width={1} />
        <Grid.Column width={14}>
            <Grid className="podium">
                <Grid.Column width={5} className="secondPlace podium-step">
                    <UserCard username={second.username} points={second.points} picture={second.picture} />
                </Grid.Column>
                <Grid.Column width={6} className="firstPlace podium-step">
                    <UserCard username={first.username} points={first.points} picture={first.picture}/>
                </Grid.Column>
                <Grid.Column width={5} className="thirdPlace podium-step">
                    <UserCard username={third.username} points={third.points} picture={third.picture} />
                </Grid.Column>
            </Grid>
        </Grid.Column>
        <Grid.Column width={1} />
    </Grid>
)