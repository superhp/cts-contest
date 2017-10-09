import React from 'react';
import UserCard from './UserCard.jsx';

export default class SpecialPrizeBoard extends React.Component {
    render() {
        return (
            <div className="col-xs-12 row">
                <div className="col-xs-1"/>
                <div className="col-xs-10">
                    <div className="col-xs-12 row">
                        <div className="col-xs-4">
                            <UserCard />
                        </div>
                        <div className="col-xs-4">
                            <UserCard />
                        </div>
                        <div className="col-xs-4">
                            
                        </div>
                    </div>
                </div>
                <div className="col-xs-1"/>
            </div>
        );
    }

}