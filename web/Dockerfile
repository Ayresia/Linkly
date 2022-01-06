FROM node:lts-alpine
WORKDIR /web
COPY package.json package-lock.json ./
RUN npm i

COPY . ./
RUN npm run build
ENTRYPOINT ["npm", "run", "start"]
