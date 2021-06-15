import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Route } from 'react-router-dom';
import Samples from './components/Pages/Samples';
import Patients from './components/Pages/Patients';
import './index.css';
import { QueryClientProvider } from 'react-query';
import { ReactQueryDevtools } from 'react-query/devtools';
import { queryClient } from './apis/constants'
import Layout from './components/Layouts/Layout'
import { QueryParamProvider } from 'use-query-params';

// const baseUrl = (document.getElementsByTagName('base')[0] || {}).href;

ReactDOM.render(
  <React.StrictMode>
    <BrowserRouter>
      <QueryParamProvider ReactRouterRoute={Route}>
        <QueryClientProvider client={queryClient}>
            <Layout >
              <Route exact path='/' component={Patients} />
              <Route path='/samples' component={Samples} />
            </Layout>
            <ReactQueryDevtools position={"bottom-right"} initialIsOpen={false} />
        </QueryClientProvider>
      </QueryParamProvider>
    </BrowserRouter>
  </React.StrictMode>,
  document.getElementById('root'),
);

// Hot Module Replacement (HMR) - Remove this snippet to remove HMR.
// Learn more: https://www.snowpack.dev/concepts/hot-module-replacement
// if (import.meta.hot) {
//   import.meta.hot.accept();
// }
