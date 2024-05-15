-- Table: public.pools

-- DROP TABLE IF EXISTS public.pools;

CREATE TABLE IF NOT EXISTS public.pools
(
    id uuid NOT NULL,
    domain text COLLATE pg_catalog."default" NOT NULL,
    port integer NOT NULL,
    cryptocurrency_id uuid NOT NULL,
    user_id bigint NOT NULL,
    CONSTRAINT pk_pools PRIMARY KEY (id),
    CONSTRAINT fk_pools_cryptocurrencies_cryptocurrency_id FOREIGN KEY (cryptocurrency_id)
        REFERENCES public.cryptocurrencies (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.pools
    OWNER to postgres;
-- Index: ix_pools_cryptocurrency_id

-- DROP INDEX IF EXISTS public.ix_pools_cryptocurrency_id;

CREATE INDEX IF NOT EXISTS ix_pools_cryptocurrency_id
    ON public.pools USING btree
    (cryptocurrency_id ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: ix_pools_user_id

-- DROP INDEX IF EXISTS public.ix_pools_user_id;

CREATE INDEX IF NOT EXISTS ix_pools_user_id
    ON public.pools USING btree
    (user_id ASC NULLS LAST)
    TABLESPACE pg_default;