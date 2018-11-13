import React from 'react';

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
        let newId = this.state.currentPosterId === 5 ? 0 : this.state.currentPosterId + 1;
        if (this.refs.didMountRef) this.setState({currentPosterId: newId});
    }

    render() {
        return <div ref="didMountRef">
            <img className="stretch-image" src={`../../img/ad${this.state.currentPosterId}.png`}/>
            </div>
    }
}