import React from 'react';
import UserCard from './UserCard.jsx';
import { Grid } from 'semantic-ui-react'

export default class SpecialPrizeBoard extends React.Component {
    render() {
        return (
                <Grid id="podiumDiv">
                    <Grid.Column width={1} />
                    <Grid.Column width={14}>
                    <Grid className="podium">
                            <Grid.Column width={5} className="secondPlace podium-step">
                                <UserCard />
                            </Grid.Column>
                            <Grid.Column width={6} className="firstPlace podium-step">
                                <UserCard />
                            </Grid.Column>
                            <Grid.Column width={5} className="thirdPlace podium-step">
                                <UserCard />
                            </Grid.Column>
                        </Grid>
                    </Grid.Column>
                    <Grid.Column width={1} />
                </Grid>
        );
    }

}