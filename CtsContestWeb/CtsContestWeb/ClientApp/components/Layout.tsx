import * as React from 'react';
import  { Header }  from './Header';
import  { Footer }  from './Footer';

export interface LayoutProps {
    children?: React.ReactNode;
}

interface UserState {
    userInfo: any;
    loading: boolean;
}

export class Layout extends React.Component<LayoutProps, UserState> {
    constructor() {
        super();
        
        this.state = { userInfo: null, loading: true };
    }

    componentDidMount() {
        fetch('api/User')
            .then(response => response.json() as Promise<UserInfo>)
            .then(data => {
                this.setState({ userInfo: data, loading: false });
            });
    }
    
    public render() {
        let header = this.state && this.state.userInfo != null ? <Header userInfo={this.state.userInfo} /> : <div></div>;

        return <div >
                    { header }
                    <div style={{ minHeight: window.innerHeight}}>
                        { this.props.children }
                    </div>
                    <Footer />
            </div>;
    }
}

