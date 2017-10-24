import React from 'react';
import { Card, Feed, Icon } from 'semantic-ui-react';
import FinalistInfo from './FinalistInfo.jsx';

export default class UserCard extends React.Component {
    render() {
        return (
            <div className="podium-user-info">
                <div className={"podium-step-content " + this.props.place}>
                    {
                        !this.props.username ? <div /> : <FinalistInfo name={this.props.username} image={this.props.picture} points={this.props.points} />
                    }
                    <WinnerTrophy />
                </div>
            </div>
        );
    }
}

const WinnerTrophy = () => {
    return (
        <Icon name='trophy' size='massive' className="podium-trophy" />
    )
}