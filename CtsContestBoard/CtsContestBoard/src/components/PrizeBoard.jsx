import React from 'react';
import PrizeCard from './PrizeCard.jsx';

export default class PrizeBoard extends React.Component {
    constructor(props) {
        super(props);
        console.log(props);
    }
    render() {
        return (
            <div>
                <div className="row prizeCardRow">
                    {this.props.prizes.map((prize, index) =>
                        <div className='col-xs-6 col-sm-4 col-md-3 col-lg-2 col-centered' key={index} style={{ paddingBottom: 20 }}>
                            <PrizeCard
                                prize={prize}
                            />
                        </div>
                    )}
                </div>
            </div>
        );
    }

}