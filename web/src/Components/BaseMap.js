import React, { PureComponent } from 'react';
import { Dropdown } from 'semantic-ui-react'
import { baseMapTool } from '../tools';

export class BaseMap extends PureComponent {

    render = () => {

        return <div className="base-map">
            <Dropdown defaultValue={baseMaps[0].value}
                // selection
                onChange={onChange}
                compact
                options={baseMaps}
            />
        </div>
    }
}

const baseMaps = [
    {
        key: 'Başar Map Yol Altlığı',
        value: 'BM',
        image: { src: '/Images/basar-map-altlik.png' },
    },
    {
        key: 'Altlıksız',
        value: 'NONE',
        image: { src: '/Images/nomap-altlik.png' },
    },
    {
        key: 'Google Yol Altlığı',
        value: 'GR',
        image: { src: '/Images/roads-google-altlik.png' },
    },
    {
        key: 'Google Uydu Altlığı',
        value: 'GS',
        image: { src: '/Images/satelite-google-altlik.png' },
    },
    {
        key: 'Google Fiziksel Altlığı',
        value: 'GP',
        image: { src: '/Images/physical-google-altlik.png' },
    }
]

const onChange = (event, data) => {

    baseMapTool.changeBaseMap(data.value);
}

export default BaseMap;