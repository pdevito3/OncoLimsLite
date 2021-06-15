import React from 'react'

function Header() {
  return (
    <header className="bg-emerald-600">
      <nav className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8" aria-label="Top">
        <div className="w-full py-3 flex items-center justify-between">
          <div className="flex items-center">
            <a href="#">
              <span className="sr-only">Workflow</span>
              <img
                className="h-10 w-auto"
                src="https://tailwindui.com/img/logos/workflow-mark.svg?color=white"
                alt=""
              />
            </a>
          </div>
          <div className="ml-10 space-x-4">
            <a
              href="/bff/login?returnUrl=/"
              className="inline-block bg-emerald-500 py-2 px-4 border border-transparent rounded-md text-base font-medium text-white hover:bg-opacity-75"
            >
              Login
            </a>
          </div>
        </div>
      </nav>
    </header>
  )
}

export default Header
