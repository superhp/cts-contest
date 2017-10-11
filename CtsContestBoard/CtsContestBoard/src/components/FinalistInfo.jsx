import React from 'react';
import { Image } from 'semantic-ui-react';

const FinalistInfo = ({ image, name, points }) => (
    <div className="podium-step-header">
        <div className="podium-step-header-image">
            <Image src={this.props.image} centered={true} shape="circular" />
        </div>
        <div className="podium-step-header-info">
            <div className="podium-step-header-info-name">
                {this.props.name}
            </div>
            <div className="podium-step-header-info-points">
                {this.props.points}
            </div>
        </div>
    </div>
)
export default FinalistInfo;