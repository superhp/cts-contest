import React from 'react';
import { Image } from 'semantic-ui-react';

const FinalistInfo = ({ image, name, points }) => (
    <div className="podium-step-header">
        <div className="podium-step-header-image">
            <Image src={image} centered={true} shape="circular" />
        </div>
        <div className="podium-step-header-info">
            <div className="podium-step-header-info-name">
                {name}
            </div>
            <div className="podium-step-header-info-points">
                {points}
            </div>
        </div>
    </div>
)
export default FinalistInfo;