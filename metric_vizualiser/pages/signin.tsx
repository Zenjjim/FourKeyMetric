import { Box, Text } from "@chakra-ui/react";
import { COLORS } from "const";
import { GetServerSidePropsContext } from "next";
import Image from "next/image";
import {
  getCsrfToken,
  getProviders,
  getSession,
  signIn,
} from "next-auth/react";
import logo from "public/logo.svg";
import ms from "public/ms.png";
import { useState } from "react";

export async function getServerSideProps(context: GetServerSidePropsContext) {
  const { req } = context;
  const session = await getSession({ req });

  if (session) {
    return {
      redirect: { destination: "/" },
    };
  }

  return {
    props: {
      providers: await getProviders(),
      csrfToken: await getCsrfToken(context),
    },
  };
}

const SignIn = ({
  providers,
}: {
  providers: { name: string; id: string }[];
}) => {
  const [hover, setHover] = useState<boolean>(false);
  return (
    <Box
      alignItems="center"
      backgroundColor={COLORS.PAPER}
      borderRadius="10px"
      display={"flex"}
      flexDirection="column"
      gap="20px"
      height="fit-content"
      justifyContent={"center"}
      left="50%"
      padding="30px"
      position={"fixed"}
      top="50%"
      transform="translate(-50%, -50%)"
      width="fit-content"
    >
      <Image
        alt="Møller Mobility Group Logo"
        height="30px"
        src={logo}
        width="300px"
      />
      <Text fontSize="4xl" fontWeight={"bold"} marginBottom="40px">
        Velkommen til 4key metrics
      </Text>
      {Object.values(providers).map((provider) => {
        return (
          <Image
            alt="Microsoft Login Button"
            height="63"
            key={provider.name}
            onClick={() => signIn(provider.id)}
            onMouseEnter={() => setHover(true)}
            onMouseLeave={() => setHover(false)}
            src={ms}
            style={{ padding: "0 70px", cursor: hover ? "pointer" : "none" }}
            width="360"
          />
        );
      })}
    </Box>
  );
};

export default SignIn;
