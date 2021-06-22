import { authenticationReducer, layerReducer, poiReducer, uiReducer } from '.';
import { combineReducers } from 'redux';

export const rootReducer = combineReducers({
    authenticationReducer,
    layerReducer,
    uiReducer,
    poiReducer,
})

export default rootReducer;