import React from 'react';

export default class PrizeBoard extends React.Component {
    constructor(props) {
        super(props);
        console.log(props);
    }
    render() {
       return (
            <div className="container">                
               <div>
                   <p>Prize board</p>
                </div>
            </div>
        )
    }

}