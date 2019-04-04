import React from 'react';

const JOB_POSTER_COUNT = 5;

export default class JobPosters extends React.Component {

    constructor() {
        super();
        this.state = {
            posterChanger: null,
            currentPosterId: 1
        };

        this.changePoster = this.changePoster.bind(this);
    }

    componentDidMount(){
        let posterChanger = setInterval(this.changePoster, 1000);
        if (this.refs.didMountRef) this.setState({posterChanger: posterChanger});
    }

    componentWillUnmount() {
        clearInterval()
    }

    changePoster() {
        const newId = this.state.currentPosterId % JOB_POSTER_COUNT + 1;
        if (this.refs.didMountRef) this.setState({currentPosterId: newId});
    }

    render() {
        return <div ref="didMountRef">
            <img className="stretch-image" src={`../../img/${this.state.currentPosterId}.png`}/>
            </div>
    }
}