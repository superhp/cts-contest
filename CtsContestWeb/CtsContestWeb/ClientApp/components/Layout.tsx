import * as React from 'react';
import { Header } from './Header';
import { Footer } from './Footer';

export interface LayoutProps {
    children?: React.ReactNode;
}

const footerHeight = 50;

export class Layout extends React.Component<LayoutProps, {}> {
    constructor() {
        super();

        this.state = { userInfo: null, loading: true };
    }


    public render() {
        return <div style={{ height: 'inherit' }}>
            <div style={{ minHeight: '100%', paddingBottom: footerHeight }}>
                <Header />
                {this.props.children}
            </div>
            <Footer height={footerHeight}/>
        </div>;
    }
}

