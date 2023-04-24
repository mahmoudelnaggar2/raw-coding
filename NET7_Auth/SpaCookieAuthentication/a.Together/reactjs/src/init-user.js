const user = {}
const userInfoRegex = new RegExp('user-info-payload=([^;]+);')
const match = userInfoRegex.exec(document.cookie)
if (match) {
    const userString = atob(match[1].replaceAll('%3D', ''))
    const userPayload = JSON.parse(userString)
    user['username'] = userPayload['username']
    document.cookie = document.cookie.replaceAll(
        /user-info-payload=([^;]+); /g,
        'user-info-payload= ; expires = Thu, 01 Jan 1970 00:00:00 GMT'
    )
}


export default user