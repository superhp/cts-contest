import React from 'react'
import { Card, Feed } from 'semantic-ui-react'

export default class UserCard extends React.Component {
    render() {
        return (
            <Card>
                <Feed>
                    <Feed.Event>
                        <Feed.Label image='https://react.semantic-ui.com/assets/images/avatar/small/jenny.jpg'/>
                        <Feed.Content>
                            <Feed.Summary>
                                Jonas Melynas
                            </Feed.Summary>
                            <Feed.Date>melejonas</Feed.Date>
                        </Feed.Content>
                    </Feed.Event>
                </Feed>
                <Card.Content>
                    
                </Card.Content>
            </Card>
        );
    }
}