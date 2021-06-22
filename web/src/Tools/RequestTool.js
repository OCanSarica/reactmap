const getRequest = (url) => {

    return handleResponse(
        fetch(url,
            {
                method: 'get',
                headers: {
                    'Content-Type': 'application/json',
                    'token': localStorage.getItem('token_')
                },
            }));
}

const postRequest = (url, data) => {

    return handleResponse(
        fetch(url,
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'token': localStorage.getItem('token_')
                },
                body: JSON.stringify(data)
            }));
}

const putRequest = (url, data) => {

    return handleResponse(
        fetch(url,
            {
                method: 'put',
                headers: {
                    'Content-Type': 'application/json',
                    'token': localStorage.getItem('token_')
                },
                body: JSON.stringify(data)
            }));
}

const deleteRequest = (url, data) => {

    return handleResponse(
        fetch(url,
            {
                method: 'delete',
                headers: {
                    'Content-Type': 'application/json',
                    'token': localStorage.getItem('token_')
                },
                body: JSON.stringify(data)
            }));
}

const handleResponse = (promise) => {

    return promise.then(response => {

        if (!response.ok) {

            if (response.status === 401) {

                window.location.href = '/login';

                return { success: false };
            }

            return Promise.reject(response.statusText);
        }

        return response.json();
    });
}

export const requestTool = {
    getRequest,
    postRequest,
    putRequest,
    deleteRequest
}

export default requestTool;