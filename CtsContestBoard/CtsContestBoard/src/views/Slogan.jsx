import React from 'react';

export default class Slogan extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div style={{ margin: 200 }}>

                <p style={{ color: '#64a70b', margin: 0, fontSize: 120, textAlign: 'center' }}>
                    Define your future 
                </p>

                <p style={{ color: '#4298b5', margin: 0, fontSize: 120, textAlign: 'center', fontWeight: 'bold' }}>
                    MAKE IT HAPPEN
                </p>
            </div>
        );
    }
}