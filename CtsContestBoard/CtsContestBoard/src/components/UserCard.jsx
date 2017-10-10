import React from 'react'
import { Card, Feed } from 'semantic-ui-react'

export default class UserCard extends React.Component {
    render() {
        return (
            <Card className="podium-user-info">
                <Feed>
                    <Feed.Event>
                        <Feed.Label image='https://i.pinimg.com/236x/ba/f9/58/baf958abca0e1e917e8520ea2dab5726--nicholas-cage-face-nicholas-dagosto.jpg'/>
                        <Feed.Content>
                            <Feed.Summary>
                                Jonas Melynas
                            </Feed.Summary>
                            <Feed.Date>melejonas</Feed.Date>
                        </Feed.Content>
                    </Feed.Event>
                </Feed>
            </Card>
        );
    }
}