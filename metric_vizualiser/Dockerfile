FROM node:16 AS builder
WORKDIR /app
COPY . .
RUN npm ci && npm run build

FROM node:16-alpine
WORKDIR /app

ENV NODE_ENV production

COPY --from=builder /app/public ./public
COPY --from=builder /app/package.json ./package.json
COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static

EXPOSE 3000
ENV PORT 3000
ENV AZURE_AD_CLIENT_ID=cbf3b43e-cc27-4ecb-abfd-f261d0c4a4de
ENV AZURE_AD_TENANT_ID=a1e38214-9521-4c2a-a29a-4fa0a4457a5c
ENV NEXT_PUBLIC_BACKEND_URL=https://weu-dev-metric-web-app.azurewebsites.net

CMD ["node", "server.js"]