import React from 'react';
import { Card, Feed, Icon } from 'semantic-ui-react';
import FinalistInfo from './FinalistInfo.jsx';

export default class UserCard extends React.Component {
    render() {
        return (
            <div className="podium-user-info">
                {/*<Feed className="podium-step-header">
                    <Feed.Event>
                        <Feed.Label image='https://i.pinimg.com/236x/ba/f9/58/baf958abca0e1e917e8520ea2dab5726--nicholas-cage-face-nicholas-dagosto.jpg'/>
                        <Feed.Content>
                            <Feed.Summary>
                                Jonas Melynas
                            </Feed.Summary>
                            <Feed.Date>melejonas</Feed.Date>
                        </Feed.Content>
                    </Feed.Event>
                </Feed>*/}
                <FinalistInfo name="Jonas Blue" image='https://i.pinimg.com/236x/ba/f9/58/baf958abca0e1e917e8520ea2dab5726--nicholas-cage-face-nicholas-dagosto.jpg' points={16}/>
                <div className="podium-step-content">
                    <WinnerTrophy />
                </div>
            </div>
        );
    }
}

const WinnerTrophy = () => {
    return (
        <Icon name='trophy' size='massive' className="podium-trophy" circular={true} />
    )
}