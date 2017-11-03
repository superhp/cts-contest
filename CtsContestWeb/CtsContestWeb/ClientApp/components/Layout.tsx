import * as React from 'react';
import { Header } from './Header';
import { HeaderMenu } from './HeaderMenu';
import { Footer } from './Footer';

export interface LayoutProps {
    children?: React.ReactNode;
}

const footerHeight = 50;

export class Layout extends React.Component<any, any> {
    constructor(props:any) {
        super(props);

        this.state = { userInfo: null, loading: true };
    }


    public render() {
        return <div style={{ height: 'inherit' }}>
            <div style={{ minHeight: '100%', paddingBottom: footerHeight }}>
                <Header userInfo={this.props.userInfo}/>
                {this.props.children}
            </div>
            {/* <Footer height={footerHeight}/> */}
        </div>;
    }
}

