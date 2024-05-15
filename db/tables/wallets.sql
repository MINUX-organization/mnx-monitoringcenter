-- Table: public.wallets

-- DROP TABLE IF EXISTS public.wallets;

CREATE TABLE IF NOT EXISTS public.wallets
(
    id uuid NOT NULL,
    name text COLLATE pg_catalog."default" NOT NULL,
    address text COLLATE pg_catalog."default" NOT NULL,
    cryptocurrency_id uuid NOT NULL,
    user_id bigint NOT NULL,
    CONSTRAINT pk_wallets PRIMARY KEY (id),
    CONSTRAINT fk_wallets_cryptocurrencies_cryptocurrency_id FOREIGN KEY (cryptocurrency_id)
        REFERENCES public.cryptocurrencies (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.wallets
    OWNER to postgres;
-- Index: ix_wallets_cryptocurrency_id

-- DROP INDEX IF EXISTS public.ix_wallets_cryptocurrency_id;

CREATE INDEX IF NOT EXISTS ix_wallets_cryptocurrency_id
    ON public.wallets USING btree
    (cryptocurrency_id ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: ix_wallets_user_id

-- DROP INDEX IF EXISTS public.ix_wallets_user_id;

CREATE INDEX IF NOT EXISTS ix_wallets_user_id
    ON public.wallets USING btree
    (user_id ASC NULLS LAST)
    TABLESPACE pg_default;