import { Dashboard } from "components/dashboard";
import { COLORS } from "const";
import {
  GetServerSideProps,
  GetServerSidePropsContext,
  InferGetServerSidePropsType,
} from "next";
import { unstable_getServerSession } from "next-auth";
import { signOut } from "next-auth/react";
import { nextAuthOptions } from "pages/api/auth/[...nextauth]";
import safeJsonStringify from "safe-json-stringify";
export const getServerSideProps: GetServerSideProps = async (
  context: GetServerSidePropsContext
) => {
  const session = await unstable_getServerSession(
    context.req,
    context.res,
    nextAuthOptions
  );
  if (!session?.accessToken) {
    return {
      redirect: {
        permanent: false,
        destination: "/signin",
      },
    };
  }
  if (!context.query?.months) {
    return {
      redirect: {
        permanent: false,
        destination: "/?months=3&organization=&project=&repository=",
      },
    };
  }
  const backend = process.env.NEXT_PUBLIC_BACKEND_URL;
  const months = context.query.months;
  const organization = context.query.organization;
  const project = context.query.project;
  const repository = context.query.repository;
  const headers = {
    headers: { Authorization: "Bearer " + session?.accessToken },
  };
  const filter = `organization=${organization}&project=${project}&repository=${repository}&intervalMonths=${months}`;
  const df = await fetch(`${backend}/DeploymentFrequency?${filter}`, headers)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
      return null;
    });
  const ltc = await fetch(`${backend}/LeadTimeChange?${filter}`, headers)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
      return null;
    });
  const cfr = await fetch(`${backend}/ChangeFailureRate?${filter}`, headers)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
      return null;
    });
  const ttrs = await fetch(`${backend}/RestoreServiceTime?${filter}`, headers)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
      return null;
    });
  const info = await fetch(`${backend}/info`, headers)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
      return null;
    });
  const safeSession = JSON.parse(safeJsonStringify(session));
  return {
    props: { df, ltc, cfr, ttrs, info, months, safeSession }, // will be passed to the page component as props
  };
};

const Home = ({
  df,
  ltc,
  cfr,
  ttrs,
  info,
  months,
  safeSession,
}: InferGetServerSidePropsType<typeof getServerSideProps>) => {
  if (safeSession) {
    return (
      <>
        <div style={{ display: "flex", width: "100%", justifyContent: "end" }}>
          <span
            style={{
              marginRight: "10px",
              alignSelf: "center",
              paddingBottom: "6px",
            }}
          >
            Signed in as <b>{safeSession.user.name}</b>
          </span>
          <button
            onClick={() => signOut()}
            style={{
              padding: "5px",
              marginBottom: "5px",
              backgroundColor: COLORS.PAPER,
              borderRadius: "4px",
            }}
          >
            Sign out
          </button>
        </div>
        <Dashboard
          cfr={cfr}
          df={df}
          info={info}
          ltc={ltc}
          months={months}
          ttrs={ttrs}
        />
      </>
    );
  }
};

export default Home;
