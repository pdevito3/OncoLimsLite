import React from 'react'


function useIsMounted() {
  const isMounted = React.useRef(true)

  React.useEffect(() => {
    return () => {
      isMounted.current = false
    }
  }, [])

  return isMounted
}

export {useIsMounted}
